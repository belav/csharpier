import * as React from "react";

const component: React.FC<{ size?: number; color?: string; className?: string }> = ({
                                                                                        size = 19,
                                                                                        color = "currentColor",
                                                                                        className,
                                                                                    }) => (
    <svg width={size} height={size} viewBox="0 0 24 24" fill="none" stroke={color}
         strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className={className}>
        <path fill="none" d="M12,22"/>
        <path fill="none" d="M12,22C6.5,22,2,17.5,2,12S6.5,2,12,2s10,4.5,10,10"/>
    </svg>
);

export const Loading = React.memo(component);